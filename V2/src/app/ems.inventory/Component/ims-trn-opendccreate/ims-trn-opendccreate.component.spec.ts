import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnOpendccreateComponent } from './ims-trn-opendccreate.component';

describe('ImsTrnOpendccreateComponent', () => {
  let component: ImsTrnOpendccreateComponent;
  let fixture: ComponentFixture<ImsTrnOpendccreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnOpendccreateComponent]
    });
    fixture = TestBed.createComponent(ImsTrnOpendccreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
