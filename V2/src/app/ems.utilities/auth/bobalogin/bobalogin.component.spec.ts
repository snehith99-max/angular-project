import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BobaloginComponent } from './bobalogin.component';

describe('BobaloginComponent', () => {
  let component: BobaloginComponent;
  let fixture: ComponentFixture<BobaloginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BobaloginComponent]
    });
    fixture = TestBed.createComponent(BobaloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
