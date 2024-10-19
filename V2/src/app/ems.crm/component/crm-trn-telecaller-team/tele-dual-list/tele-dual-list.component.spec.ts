import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeleDualListComponent } from './tele-dual-list.component';

describe('TeleDualListComponent', () => {
  let component: TeleDualListComponent;
  let fixture: ComponentFixture<TeleDualListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TeleDualListComponent]
    });
    fixture = TestBed.createComponent(TeleDualListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
