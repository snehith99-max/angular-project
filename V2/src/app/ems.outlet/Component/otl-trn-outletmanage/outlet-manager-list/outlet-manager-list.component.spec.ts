import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutletManagerListComponent } from './outlet-manager-list.component';

describe('OutletManagerListComponent', () => {
  let component: OutletManagerListComponent;
  let fixture: ComponentFixture<OutletManagerListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OutletManagerListComponent]
    });
    fixture = TestBed.createComponent(OutletManagerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
