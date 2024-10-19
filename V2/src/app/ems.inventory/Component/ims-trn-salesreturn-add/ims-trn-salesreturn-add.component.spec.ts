import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnSalesreturnAddComponent } from './ims-trn-salesreturn-add.component';

describe('ImsTrnSalesreturnAddComponent', () => {
  let component: ImsTrnSalesreturnAddComponent;
  let fixture: ComponentFixture<ImsTrnSalesreturnAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnSalesreturnAddComponent]
    });
    fixture = TestBed.createComponent(ImsTrnSalesreturnAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
