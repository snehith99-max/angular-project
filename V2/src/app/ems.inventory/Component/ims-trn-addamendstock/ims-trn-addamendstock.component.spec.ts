import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnAddamendstockComponent } from './ims-trn-addamendstock.component';

describe('ImsTrnAddamendstockComponent', () => {
  let component: ImsTrnAddamendstockComponent;
  let fixture: ComponentFixture<ImsTrnAddamendstockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnAddamendstockComponent]
    });
    fixture = TestBed.createComponent(ImsTrnAddamendstockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
