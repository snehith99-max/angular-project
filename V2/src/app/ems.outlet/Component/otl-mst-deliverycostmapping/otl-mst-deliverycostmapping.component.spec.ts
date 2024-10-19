import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstDeliverycostmappingComponent } from './otl-mst-deliverycostmapping.component';

describe('OtlMstDeliverycostmappingComponent', () => {
  let component: OtlMstDeliverycostmappingComponent;
  let fixture: ComponentFixture<OtlMstDeliverycostmappingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstDeliverycostmappingComponent]
    });
    fixture = TestBed.createComponent(OtlMstDeliverycostmappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
