import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnOutletdirectpoComponent } from './otl-trn-outletdirectpo.component';

describe('OtlTrnOutletdirectpoComponent', () => {
  let component: OtlTrnOutletdirectpoComponent;
  let fixture: ComponentFixture<OtlTrnOutletdirectpoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnOutletdirectpoComponent]
    });
    fixture = TestBed.createComponent(OtlTrnOutletdirectpoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
